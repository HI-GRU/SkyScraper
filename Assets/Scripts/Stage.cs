public class Stage
{
    public int SizeX { get; private set; }
    public int SizeZ { get; private set; }
    public Building[] buildings { get; private set; }
    public string stageName { get; private set; }

    public Stage(int SizeX, int SizeZ, Building[] buildings, string stageName)
    {
        this.SizeX = SizeX;
        this.SizeZ = SizeZ;
        this.buildings = buildings;
        this.stageName = stageName;
    }
}