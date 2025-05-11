namespace SiemensInternship.Model
{
    public class UndoAction
    {
        public UndoActionType ActionType { get; set; }
        public Book Book { get; set; }
        public Book PreviousBook { get; set; }
    }

    public enum UndoActionType
    {
        Create,
        Update,
        Delete,
    }
}
