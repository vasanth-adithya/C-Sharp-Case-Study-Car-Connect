using System;
using System.Collections.Generic;
using CarConnect.Entity;

namespace CarConnect.DAO
{
    internal interface IReservationService
    {
        List<Reservation> GetAllReservations();

        void GetReservationById(int reservationId);

        void GetReservationByCustomerId(int customerId);

        string CreateReservation(Reservation reservationData);

        string UpdateReservation(Reservation reservationData);

        string CancelReservation(int reservationId);

        decimal CalculateTotalCost(DateTime startDate, DateTime endDate, int vehicleId);
    }
}