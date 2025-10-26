namespace Packt.CloudySkiesAir.Chapter4;

public class FlightTracker {
    private readonly List<Flight> _flights = new();

    public Flight ScheduleNewFlight(string id, string dest, DateTime depart) {
        Flight flight = new() {
            Id = id,
            Destination = dest,
            DepartureTime = depart,
            Status = FlightStatus.Inbound
        };
        _flights.Add(flight);
        return flight;
    }

    public void DisplayFlights() {
        foreach (Flight f in _flights) {
            Console.WriteLine($"{f.Id,-9} {f.Destination,-5} {Format(f.DepartureTime),-21} {f.Gate,-5} {f.Status}");
        }
    }

    public void DisplayMatchingFlights(List<Flight> flights,Func<Flight,bool> shouldDisplay) {
        foreach (var flight in flights) {
            if (shouldDisplay(flight)) {
                Console.WriteLine(flight);
            }
        }
    }

    public Flight? MarkFlightDelayed(string id, DateTime newDepartureTime) {
        Action<Flight> updateAction = (flight) => {
            flight.DepartureTime = newDepartureTime;
            flight.Status = FlightStatus.Delayed;
            Console.WriteLine($"{id} delayed until {Format(newDepartureTime)}");
        };
        return UpdateFlight(id, updateAction);
    }

    public Flight? MarkFlightArrived(string id, DateTime arrivalTime, string gate) {
        Flight? flight = FindFlightById(id);
        if (flight != null) {
            flight.ArrivalTime = arrivalTime;
            flight.Status = FlightStatus.OnTime;
            flight.Gate = gate;
            Console.WriteLine($"{id} arrived at {Format(arrivalTime)}.");
        } else {
            Console.WriteLine($"{id} could not be found");
        }
        return flight;
    }

    public Flight? MarkFlightDeparted(string id, DateTime depatureTime) {
        Flight? flight = FindFlightById(id);
        if (flight != null) {
            flight.DepartureTime = depatureTime;
            flight.Status = FlightStatus.Departed;
            Console.WriteLine($"{id} departed at {Format(depatureTime)}.");
        } else {
            Console.WriteLine($"{id} could not be found");
        }
        return flight;
    }

    public Flight? FindFlightById(string id) => _flights.FirstOrDefault(f => f.Id == id);

    private string Format(DateTime time) {
        return time.ToString("ddd MMM dd HH:mm tt");
    }

    private Flight? UpdateFlight(string id, Action<Flight> updateAction) {
        Flight? flight = FindFlightById(id);
        if (flight != null) { 
            updateAction(flight);
        } else {
            Console.WriteLine($"{id} could not be found");
        }
        return flight;
    }
}