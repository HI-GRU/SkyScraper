public class Building
{
    public int SizeX { get; private set; }
    public int SizeY { get; private set; } // 높이
    public int SizeZ { get; private set; }

    public Building(int SizeX, int SizeY, int SizeZ)
    {
        this.SizeX = SizeX;
        this.SizeY = SizeY;
        this.SizeZ = SizeZ;
    }

}