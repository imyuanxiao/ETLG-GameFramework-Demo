
namespace ETLG.Data
{
    public abstract class ArtifactDataBase
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public string Type { get; protected set; }
        public bool Tradeable { get; protected set; }
        public int Value { get; protected set; }
        public int MaxNumber { get; protected set; }

    }

}
