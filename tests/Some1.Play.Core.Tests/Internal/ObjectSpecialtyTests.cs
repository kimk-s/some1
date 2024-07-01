using Some1.Play.Info;

namespace Some1.Play.Core.Internal;

public class ObjectSpecialtyTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public void Ctor(int index)
    {
        var specialty = new ObjectSpecialty(index);

        Assert.Equal(index, specialty.Index);
        Assert.Null(specialty.Id.CurrentValue);
        Assert.Equal(0, specialty.Number.CurrentValue);
        Assert.Equal(0, specialty.Token.CurrentValue);
    }

    [Fact]
    public void Add()
    {
        var specialty = new ObjectSpecialty(0);

        {
            specialty.Add(SpecialtyId.Mob1, 1, 1);

            Assert.Equal(SpecialtyId.Mob1, specialty.Id.CurrentValue);
            Assert.Equal(1, specialty.Number.CurrentValue);
            Assert.Equal(1, specialty.Token.CurrentValue);
        }

        {
            specialty.Add(SpecialtyId.Mob1, 2, 2);

            Assert.Equal(SpecialtyId.Mob1, specialty.Id.CurrentValue);
            Assert.Equal(3, specialty.Number.CurrentValue);
            Assert.Equal(2, specialty.Token.CurrentValue);
        }
    }

    [Fact]
    public void Add_ZeroNumber_Throws()
    {
        var specialty = new ObjectSpecialty(0);

        Assert.Throws<ArgumentOutOfRangeException>(() => specialty.Add(SpecialtyId.Mob1, 0, 1));
    }

    [Fact]
    public void Add_SpecialtyIdMistmatch_SetNumber()
    {
        var specialty = new ObjectSpecialty(0);
        specialty.Add(SpecialtyId.Mob1, 1, 1);

        specialty.Add(SpecialtyId.Mob2, 1, 2);

        Assert.Equal(SpecialtyId.Mob2, specialty.Id.CurrentValue);
        Assert.Equal(1, specialty.Number.CurrentValue);
        Assert.Equal(2, specialty.Token.CurrentValue);
    }

    [Fact]
    public void Reset()
    {
        var specialty = new ObjectSpecialty(0);
        specialty.Add(SpecialtyId.Mob1, 1, 1);

        specialty.Reset();

        Assert.Equal(0, specialty.Index);
        Assert.Null(specialty.Id.CurrentValue);
        Assert.Equal(0, specialty.Number.CurrentValue);
        Assert.Equal(0, specialty.Token.CurrentValue);
    }
}
