using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatTracker.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PreferedPosition { get; set; }
        public int? PreferedJerseyNumber { get; set; }
        public int? SkillLevel { get; set; }
        public DateTime? DueBackDate { get; set; }

        public string FriendlyDueBackDate
        {
            get
            {
                if (DueBackDate.HasValue)
                {

                    return ((DateTime)this.DueBackDate).ToShortDateString();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}