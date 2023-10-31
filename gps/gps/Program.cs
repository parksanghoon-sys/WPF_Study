using Windows.ApplicationModel.Background;
using Windows.Devices.Geolocation;

public class Program
{
    public static  void Main(string[] arg)
    {
        GetLocation();
        Console.ReadLine();
    }
    private static async void GetLocation()
    {
        var accessStatus = await Geolocator.RequestAccessAsync();

        switch (accessStatus)
        {
            case GeolocationAccessStatus.Allowed:
                Geolocator geolocator = new Geolocator();
                Geoposition pos = await geolocator.GetGeopositionAsync();
                UpdateLocationData(pos);
                break;

            case GeolocationAccessStatus.Denied:
                Console.WriteLine("Access to location is denied.");
                break;

            case GeolocationAccessStatus.Unspecified:
                Console.WriteLine("Unspecified error.");
                break;
        }
    }
    private static void UpdateLocationData(Geoposition position)
    {
        Console.WriteLine($"Latitude: {position.Coordinate.Latitude}, Longitude: {position.Coordinate.Longitude}");
    }
}