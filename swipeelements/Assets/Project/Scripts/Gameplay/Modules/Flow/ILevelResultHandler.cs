namespace Project.Gameplay
{
    public interface ILevelResultHandler
    {
        public void Restart();
        public void Skip();
        public void Complete();
    }
}