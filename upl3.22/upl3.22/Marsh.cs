public class MARSH
{
    public string StartPoint { get; set; }
    public string EndPoint { get; set; }
    public int RouteNumber { get; set; }

    public override string ToString()
    {
        return $"Маршрут №{RouteNumber}: {StartPoint} -> {EndPoint}";
    }
}