using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Some1.Data.InMemory.Internal;
using Some1.Play.Data;
using Some1.Wait.Data;

namespace Some1.Data.InMemory
{
    public sealed class InMemoryRepository
    {
        private readonly InMemoryRepositoryOptionsUser? _userOptions;
        private readonly Dictionary<string, UserData> _usersById = new();
        private readonly WaitData _wait;
        private readonly Dictionary<string, PlayData> _playsById;
        private readonly List<PremiumLogData> _premiumLogs = new();
        private readonly HashSet<(WaitPremiumLogReason reason, string purchaseOrderId)> _premiumLogUniqueKey = new();

        public InMemoryRepository(IOptions<InMemoryRepositoryOptions> options)
        {
            _userOptions = options.Value.User;

            _wait = new()
            {
                Maintenance = options.Value.Wait?.Maintenance ?? false
            };

            _playsById = options.Value.Plays
                .Select(x => new PlayData()
                {
                    Id = x.Id,
                    Region = x.Region,
                    City = x.City,
                    Number = x.Number,
                    Address = x.Address,
                    OpeningSoon = x.OpeningSoon,
                    Maintenance = x.Maintenance,
                    Busy = x.Busy,
                })
                .ToDictionary(x => x.Id);
        }

        public WaitUserData GetWaitUser(string id)
        {
            var user = GetOrCreateUser(id);
            return new()
            {
                Id = user.Id,
                PlayId = user.PlayId,
                IsPlaying = user.IsPlaying,
                Manager = user.Manager,
            };
        }

        public WaitWaitData GetWaitWait()
        {
            return new()
            {
                Maintenance = _wait.Maintenance,
            };
        }

        public WaitPlayData[] GetWaitPlays()
        {
            return _playsById.Values
                .Select(x => new WaitPlayData
                {
                    Id = x.Id,
                    Region = x.Region,
                    City = x.City,
                    Number = x.Number,
                    Address = x.Address,
                    OpeningSoon= x.OpeningSoon,
                    Maintenance = x.Maintenance,
                    Busy = x.Busy,
                })
                .ToArray();
        }

        public PlayUserData[] LoadPlayUsers(IReadOnlyList<string> ids, string playId)
        {
            var users = new PlayUserData[ids.Count];
            for (int i = 0; i < ids.Count; i++)
            {
                var user = GetOrCreateUser(ids[i]);

                if (user.PlayId == playId || !user.IsPlaying)
                {
                    user.PlayId = playId;
                    user.IsPlaying = true;
                }

                users[i] = new PlayUserData
                {
                    Id = user.Id,
                    PlayId = user.PlayId,
                    IsPlaying = user.IsPlaying,
                    Manager = user.Manager,
                    Premium = user.PremiumExpirationDate,
                    PackedData = user.PackedData,
                };
            }
            return users;
        }

        public PlaySaveUserResult[] SavePlayUsers(IReadOnlyList<IPlayUserData> players)
        {
            var results = new PlaySaveUserResult[players.Count];

            var utcNow = DateTime.UtcNow;

            for (int i = 0; i < players.Count; i++)
            {
                var item = players[i];

                var user = _usersById[item.Id];

                if (user.PlayId == item.PlayId || !user.IsPlaying)
                {
                    user.PlayId = item.PlayId;
                    user.IsPlaying = item.IsPlaying;
                    user.PackedData = null; // Not support
                    user.SaveTime = utcNow;
                }

                results[i] = new(
                    user.Id,
                    user.PremiumExpirationDate);
            }

            return results;
        }

        public PlayPlayData GetPlayPlay(string id)
        {
            var item = _playsById[id];

            return new()
            {
                Id = item.Id,
                Maintenance = item.Maintenance,
            };
        }

        public void SetPlayPlay(PlayPlayData play)
        {
            var item = _playsById[play.Id];
            item.Busy = play.Busy;
            item.SetTime = DateTime.UtcNow;
        }

        public WaitPremiumLogData[] GetPremiumLogs(string userId)
        {
            return _premiumLogs
                .Where(x => x.UserId == userId)
                .Select(x => new WaitPremiumLogData()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Reason = x.Reason,
                    CreatedDate = x.CreatedDate,
                    PremiumChangedDays = x.PremiumChangedDays,
                    PremiumExpirationDate = x.PremiumExpirationDate,
                    PurchaseOrderId = x.PurchaseOrderId,
                    PurchaseProductId = x.PurchaseProductId,
                    PurchaseDate = x.PurchaseDate,
                    Note = x.Note,
                })
                .ToArray();
        }

        public void BuyProduct(string userId, string productId, string orderId, DateTime purchaseDate, int premium)
        {
            if (!_usersById.TryGetValue(userId, out var user))
            {
                return;
            }

            if (!_premiumLogUniqueKey.Add((WaitPremiumLogReason.Purchase, orderId)))
            {
                return;
            }

            var utcNow = DateTime.UtcNow;
            DateTime p = user.PremiumExpirationDate ?? DateTime.MinValue;
            bool continuous = p > utcNow;
            DateTime baseTime = continuous ? p : utcNow;
            user.PremiumExpirationDate = baseTime.AddDays(premium);

            user.PremiumAccumulatedDays += premium;

            var log = new PremiumLogData()
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Reason = WaitPremiumLogReason.Purchase,
                CreatedDate = utcNow,
                PremiumChangedDays = premium,
                PremiumAccumulatedDays = user.PremiumAccumulatedDays,
                PremiumExpirationDate = user.PremiumExpirationDate.Value,
                PurchaseOrderId = orderId,
                PurchaseProductId = productId,
                PurchaseDate = purchaseDate,
            };
            _premiumLogs.Add(log);
        }

        private UserData GetOrCreateUser(string id)
        {
            if (!_usersById.TryGetValue(id, out var user))
            {
                user = new()
                {
                    Id = id,
                    PlayId = _userOptions?.PlayId ?? "",
                    IsPlaying = _userOptions?.IsPlaying ?? false,
                    Manager = _userOptions?.Manager ?? false,
                    CreatedTime = DateTime.UtcNow,
                };

                _usersById.Add(user.Id, user);
            }

            return user;
        }
    }
}
