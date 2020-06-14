﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WhyNotEarth.Meredith.Validation;

namespace WhyNotEarth.Meredith.Models
{
    public abstract class ProductModel
    {
        [NotNull]
        [Mandatory]
        public int? PageId { get; set; }

        [NotNull]
        [Mandatory]
        public decimal? Price { get; set; }

        [NotNull]
        [Mandatory]
        public string? Name { get; set; }

        public string? ImageURL { get; set; }

        public List<ProductLocationInventoryModel>? LocationInventories { get; set; }

        public List<VariationModel>? Variations { get; set; }

        public List<ProductAttributeModel>? Attributes { get; set; }
    }

    public class ProductLocationInventoryModel
    {
        [NotNull]
        [Mandatory]
        public int? LocationId { get; set; }

        [NotNull]
        [Mandatory]
        public int? Count { get; set; }
    }

    public class VariationModel
    {
        [NotNull]
        [Mandatory]
        public string? Name { get; set; }

        [NotNull]
        [Mandatory]
        public decimal? Price { get; set; }
    }

    public class ProductAttributeModel
    {
        [NotNull]
        [Mandatory]
        public string? Name { get; set; }

        [NotNull]
        [Mandatory]
        public decimal? Price { get; set; }
    }
}