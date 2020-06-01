using System;
using System.Collections.Generic;
using PrimeService.Domain;

namespace PrimeService.DataInterface
{
    public interface IDeskRepository
    {
       IEnumerable<Desk> GetAvailableDesks(DateTime date);
    }
}