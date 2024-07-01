using Some1.Play.Core.TestHelpers;
using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class ObjectSpecialtyGroupTests
{
    [Fact]
    public void Ctor()
    {
        var s = new Services(2);

        Assert.Equal(Enumerable.Range(0, 2), s.Specialties.All.Select(x => x.Index));
    }

    [Fact]
    public void Add()
    {
        var s = new Services();

        {
            s.Specialties.Add(SpecialtyId.Mob1, 1);

            Assert.Equal(1, s.Specialties.All[0].Number.CurrentValue);
            Assert.Equal(1, s.Specialties.All[0].Token.CurrentValue);
        }

        {
            s.Specialties.Add(SpecialtyId.Mob1, 2);

            Assert.Equal(3, s.Specialties.All[0].Number.CurrentValue);
            Assert.Equal(2, s.Specialties.All[0].Token.CurrentValue);
        }

        {
            s.Specialties.Add(SpecialtyId.Mob2, 1);

            Assert.Equal(1, s.Specialties.All[1].Number.CurrentValue);
            Assert.Equal(3, s.Specialties.All[1].Token.CurrentValue);
        }

        {
            s.Specialties.Add(SpecialtyId.Mob2, 2);

            Assert.Equal(3, s.Specialties.All[1].Number.CurrentValue);
            Assert.Equal(4, s.Specialties.All[1].Token.CurrentValue);
        }

        {
            s.Specialties.Add(SpecialtyId.Mob3, 1);

            Assert.Equal(1, s.Specialties.All[0].Number.CurrentValue);
            Assert.Equal(5, s.Specialties.All[0].Token.CurrentValue);
        }
    }

    [Fact]
    public void Reset()
    {
        var s = new Services();
        s.Specialties.Add(SpecialtyId.Mob1, 1);
        s.Specialties.Add(SpecialtyId.Mob2, 1);

        s.Specialties.Reset();

        Assert.All(s.Specialties.All, x => Assert.Equal((null, 0, 0), (x.Id.CurrentValue, x.Number.CurrentValue, x.Token.CurrentValue)));
    }

    private sealed class Services
    {
        public Services() : this(2)
        {
        }

        public Services(int count)
        {
            var clock = new FakeClock();
            var time = new Time(clock);
            var stuffs = new ObjectTakeStuffGroup(1, time);
            Specialties = new(count, stuffs)
            {
                Enabled = true
            };
        }

        public ObjectSpecialtyGroup Specialties { get; }
    }
}
