namespace EightBitSaxLounge.Composer.Mxl.Models.MusicTheory
{
    public class ChordGenerator
    {
        private static readonly Dictionary<int, string[]> CircleOfFifthsChords = new Dictionary<int, string[]>
        {
            { -7, new[] { "Cb", "Dbm", "Ebm", "Fb", "Gb", "Abm", "Bbm" } },
            { -6, new[] { "Gb", "Abm", "Bbm", "Cb", "Db", "Ebm", "Fm" } },
            { -5, new[] { "Db", "Ebm", "Fm", "Gb", "Ab", "Bbm", "Cm" } },
            { -4, new[] { "Ab", "Bbm", "Cm", "Db", "Eb", "Fm", "Gm" } },
            { -3, new[] { "Eb", "Fm", "Gm", "Ab", "Bb", "Cm", "Dm" } },
            { -2, new[] { "Bb", "Cm", "Dm", "Eb", "F", "Gm", "Am" } },
            { -1, new[] { "F", "Gm", "Am", "Bb", "C", "Dm", "Em" } },
            { 0, new[] { "C", "Dm", "Em", "F", "G", "Am", "Bm" } },
            { 1, new[] { "G", "Am", "Bm", "C", "D", "Em", "F#m" } },
            { 2, new[] { "D", "Em", "F#m", "G", "A", "Bm", "C#m" } },
            { 3, new[] { "A", "Bm", "C#m", "D", "E", "F#m", "G#m" } },
            { 4, new[] { "E", "F#m", "G#m", "A", "B", "C#m", "D#m" } },
            { 5, new[] { "B", "C#m", "D#m", "E", "F#", "G#m", "A#m" } },
            { 6, new[] { "F#", "G#m", "A#m", "B", "C#", "D#m", "E#m" } },
            { 7, new[] { "C#", "D#m", "E#m", "F#", "G#", "A#m", "B#m" } }
        };

        public static string[] GetChords(int circleOfFifths)
        {
            return CircleOfFifthsChords[circleOfFifths];
        }

        public static string AddChordLocation(string chord, int keyAsFifths, bool relativeMinor)
        {
            var chords = GetChords(keyAsFifths);
            var chordIndex = Array.IndexOf(chords, chord);
            if (relativeMinor)
            {
                chordIndex = (chordIndex + chords.Length - 5) % chords.Length;
            }
            return $"{chord} ({chordIndex + 1})";
        }
    }
}