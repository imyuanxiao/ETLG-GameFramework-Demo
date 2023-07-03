
using System.Collections.Generic;

namespace ETLG.Data
{

  
  public sealed class PlayerArtifactData
    {
        public int Id { get; set; }
        public int Number { get; set; }

        public string Type { get; set; }

        public PlayerArtifactData(ArtifactDataBase artifactDataBase)
        {
            this.Id = artifactDataBase.Id; 
            this.Type = artifactDataBase.Type;
        }

    }

}

