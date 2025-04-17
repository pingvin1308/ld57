namespace Game.Scripts.Dialog
{
    [System.Serializable]
    public class DialogChoice
    {
        public string Text { get; set; }
        public DialogNode NextNode { get; set; }
    }
}