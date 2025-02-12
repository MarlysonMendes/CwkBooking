﻿using AutoMapper;
using CwkBooking.Api.Dtos;
using CwkBooking.Domain.Abstractions.Repositories;
using CwkBooking.Domain.Abstractions.Services;
using CwkBooking.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CwkBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResevationsController : ControllerBase
    {
        private readonly IReservationService _reservationsService;
        private readonly IMapper _mapper;
        private readonly IReservationsRepository _reservationRepo;
        public ResevationsController(IReservationService reservationService, IReservationsRepository ReservationRepo, IMapper mapper)
        {
            _reservationsService = reservationService;
            _mapper = mapper;
            _reservationRepo = ReservationRepo;
        }

        [HttpPost]
        public async Task<IActionResult> MakeReservation([FromBody] ReservationPutPostDto reservationDto)
        {
            var reservation = _mapper.Map<Reservation>(reservationDto);
            var result = await _reservationsService.MakeReservation(reservation);

            if (result == null)
                return BadRequest("Cannot create reservation");

            var mapped = _mapper.Map<ReservationGetDto>(result);

            return Ok(mapped);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await _reservationRepo.GetAllReservationsAsync();
            var mapped = _mapper.Map<List<ReservationGetDto>>(reservations);

            return Ok(mapped);
        }

        [HttpGet]
        [Route("{reservationdId}")]
        public async Task<IActionResult> GetReservationById(int reservationdId)
        {
            var reservation = await _reservationRepo.GetReservationByIdAsync(reservationdId);
            if (reservation == null)
                return NotFound($"No reservation found for the id: {reservationdId}");

            var mapped = _mapper.Map<ReservationGetDto>(reservation);
            return Ok(mapped);
        }

        [HttpDelete]
        [Route("{reservationId}")]
        public async Task<IActionResult> CancelReservation(int reservationId)
        {
            var deleted = await _reservationRepo.DeleteReservationAsync(reservationId);
            if (deleted == null) return NotFound();

            return NoContent();
        }
    }
}