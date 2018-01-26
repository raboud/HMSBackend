﻿using System.ComponentModel.DataAnnotations.Schema;

namespace HMS.Catalog.API.Model
{
	public class Brand
    {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

        public string Name { get; set; }
		public bool InActive { get; set; }
    }
}