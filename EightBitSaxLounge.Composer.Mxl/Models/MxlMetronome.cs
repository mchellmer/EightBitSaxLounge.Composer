namespace EightBitSaxLounge.Composer.Mxl.Models;

public class MxlMetronome
{
    public int Staff { get; set; }
    public int ChordTransitionDuration { get; set; }
    public int Location { get; set; }

    public MxlMetronome(int staff)
    {
        Location = 0;
        LastLocation = 0;
        ChordTransitionDuration = 0;
        Staff = staff;
    }

    public int LastLocation { get; set; }
}