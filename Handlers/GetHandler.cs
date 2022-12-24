using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketAPI_Data;
using TicketAPI_Models;

namespace TicketAPI_Handlers
{
    public class GetHandler
    {
        private readonly string connString;

        public GetHandler(string connString)
        {
            this.connString = connString;
        }

        // Methods
        public List<Ticket> GetAllTickets(SqlRepo repo)
        {
            return repo.getAllTickets(connString);
        }

        public List<Ticket> GetPendingTickets(SqlRepo repo)
        {
            return repo.getPendingTickets(connString);
        }

        public ActionResult<List<Ticket>> GetTicketById(SqlRepo repo, int ticketId)
        {
            List<Ticket>? response = repo.getTicketById(connString, ticketId);
            return response;

        }

        public ActionResult<List<Ticket>> GetSinglePending(SqlRepo repo, int ticketId)
        {
            List<Ticket>? response = repo.getSinglePending(connString, ticketId);
            return response;
        }


        public List<Ticket> GetUserTickets(SqlRepo repo, int userId)
        {
            List<Ticket>? response = repo.getUserTickets(connString, userId);
            return (response.Count >= 1) ? response : throw new NotFoundException();
        }
    }
}
