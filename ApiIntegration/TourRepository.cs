﻿using ApiIntegration.Interfaces;
using ApiIntegration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ApiIntegration
{
    public class TourRepository : ITourRepository
    {
        private Dictionary<int, Tour> tours;

        public TourRepository()
        {
            this.tours = new Dictionary<int, Tour>()
            {
                { 1, new Tour()
                    {
                        TourId = 1,
                        TourRef = "EUR123",
                        TourName = "Cycling Danube",
                        ProviderId = 1,
                        Active = true,
                        ReviewCount = 13,
                        ReviewScore = 4.3m,
                        Availabilities = new List<TourAvailability>()
                        {
                            new TourAvailability()
                            {
                                TourId = 1,
                                SellingPrice = 500,
                                StartDate = new DateTime(2020, 6, 20),
                                TourDuration = 6,
                                AvailabilityCount = 9
                            },
                            new TourAvailability()
                            {
                                TourId = 1,
                                SellingPrice = 450,
                                StartDate = new DateTime(2020, 6, 27),
                                TourDuration = 6,
                                AvailabilityCount = 9
                            }
                        }
                    } },
                { 2, new Tour()
                    {
                        TourId = 2,
                        TourRef = "EUR456",
                        TourName = "Cycling Rhine",
                        ProviderId = 1,
                        Active = true,
                        ReviewCount = 55,
                        ReviewScore = 4.8m,
                        Availabilities = new List<TourAvailability>()
                        {
                            new TourAvailability()
                            {
                                TourId = 2,
                                SellingPrice = 720,
                                StartDate = new DateTime(2020, 3, 10),
                                TourDuration = 11,
                                AvailabilityCount = 4
                            },
                            new TourAvailability()
                            {
                                TourId = 2,
                                SellingPrice = 720,
                                StartDate = new DateTime(2020, 3, 20),
                                TourDuration = 11,
                                AvailabilityCount = 5
                            }
                        }
                    }
                }
            };
        }

        public async Task<IEnumerable<Tour>> Get(Expression<Func<Tour, bool>> filter = null, Func<IQueryable<Tour>, IOrderedQueryable<Tour>> orderBy = null)
        {
            IQueryable<Tour> query = this.tours.Values.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                return orderBy(query).AsEnumerable();
            }
            else
            {
                return query.AsEnumerable();
            }
        }

        public async Task<Tour> Get(int tourId, string tourRef)
        {
            Tour tour;
            if (tourId != default && this.tours.ContainsKey(tourId))
            {
                tour = this.tours[tourId];
            }
            else if (!string.IsNullOrWhiteSpace(tourRef))
            {
                tour = tours.Values
                    .SingleOrDefault(t => t.TourRef.Equals(tourRef, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                tour = null;
            }

            return tour;
        }

        public Task Update(Tour tour)
        {
            if (tour.TourId != default
                    && tours.ContainsKey(tour.TourId))
            {
                tours[tour.TourId] = tour;
            }
            else
            {
                throw new Exception($"Tour with TourId: {tour.TourId} does not exist");
            };

            return Task.CompletedTask;
        }
    }
}
