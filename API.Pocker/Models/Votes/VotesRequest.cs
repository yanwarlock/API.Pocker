﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Pocker.Models.Votes
{
    public class VotesRequest
    {
        public string UserId { get; set; }
        public string CardId { get; set; }
        public string HistoryId { get; set; }
    }
}
