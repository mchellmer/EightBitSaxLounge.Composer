namespace EightBitSaxLounge.Composer.Mxl.Models.MusicTheory;

public static class MeasureAnalyzer
{
    /// <summary>
    /// Chord naming is subjective, but strong indicators are
    /// - the bass note at the start of a measure part being the root
    /// - the presence of the third note in the scale
    /// - the presence of the fifth note in the scale
    /// - notes on the downbeat are stronger indicators as upbeats commonly have passing tones
    /// - start by assuming that the earliest bass note is the root
    /// </summary>
    /// <param name="mxlMeasure"></param>
    /// <returns></returns>
    public static List<string> DetermineChordsInMeasure(MxlMeasure mxlMeasure)
    {
        var chords = new List<string>();
        var potentialChords = ChordGenerator.GetChords(mxlMeasure.Key);
        
        var downbeatLocations = Enumerable.Range(0, mxlMeasure.BeatCount)
            .Select(beat => beat * (mxlMeasure.Divisions / (mxlMeasure.BeatType / mxlMeasure.BeatCount)))
            .ToList();

        var notesAtDownbeatLocations = mxlMeasure.Notes
            .Where(note => downbeatLocations.Contains(note.Location))
            .ToList();
        
        var earliestBassNote = notesAtDownbeatLocations
            .Where(note => note.Pitch != null)
            .OrderBy(note => note.Pitch.Octave)
            .ThenBy(note => "CDEFGAB".IndexOf(note.Pitch.Step.ToString()))
            .ThenBy(note => note.Pitch.Alter)
            .ThenBy(note => note.Location)
            .FirstOrDefault();
        
        var earliestChord = potentialChords
            .FirstOrDefault(chord => 
            {
                var rootNote = chord.Length > 1 && (chord[1] == '#' || chord[1] == 'b') ? chord.Substring(0, 2) : chord.Substring(0, 1);
                var alter = rootNote.Length > 1 && rootNote[1] == '#' ? 1 : rootNote.Length > 1 && rootNote[1] == 'b' ? -1 : 0;
                return earliestBassNote != null && earliestBassNote.Pitch.Step.ToString() == rootNote[0].ToString() && earliestBassNote.Pitch.Alter == alter;
            });
        
        chords.Add(earliestChord);
        
        return chords;
    }
}