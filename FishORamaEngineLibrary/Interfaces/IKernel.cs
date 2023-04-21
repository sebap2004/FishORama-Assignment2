namespace FishORamaEngineLibrary
{
    public interface IKernel
    {
        Screen Screen { get; }

        void InsertToken(IDraw pToken);
        void RemoveToken(IDraw pToken);

        public void UpdateScoreText(int pTeam, int pScore);
    }
}
