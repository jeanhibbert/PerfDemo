readonly record struct TaxiRide(string VendorId, RateCodes RateCode, byte PassengerCount, short TripTimeInSecs, double TripDistance, string PaymentType, decimal FareAmount) {

    public override string ToString() => $"{VendorId},{RateCode},{PassengerCount},{TripTimeInSecs},{TripDistance:N2},{PaymentType},{FareAmount:C2}";
}
