namespace AuctionExpert.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AuctionExpert.Data.Models;

    internal class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            var categories = this.SeedCategories();

            await dbContext.Categories.AddRangeAsync(categories);
            await dbContext.SaveChangesAsync();
        }

        private HashSet<Category> SeedCategories()
        {
            return new HashSet<Category>
            {
                new Category()
                {
                    Name = "Archaeology & Natural History",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Archaeology",
                        },
                        new SubCategory()
                        {
                            Name = "Minerals & Meteorites",
                        },
                        new SubCategory()
                        {
                            Name = "Fossils",
                        },
                    },
                },
                new Category()
                {
                    Name = "Art",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Classical Art",
                        },
                        new SubCategory()
                        {
                            Name = "Modern Art",
                        },
                        new SubCategory()
                        {
                            Name = "Photography",
                        },
                        new SubCategory()
                        {
                            Name = "Posters",
                        },
                    },
                },
                new Category()
                {
                    Name = "Cars, Motorcycles & Automobiles",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Classic Cars",
                        },
                        new SubCategory()
                        {
                            Name = "Classic Motorcycles",
                        },
                        new SubCategory()
                        {
                            Name = "Modern Cars & Motorcycles",
                        },
                    },
                },
                new Category()
                {
                    Name = "Coins",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Ancient Coins",
                        },
                        new SubCategory()
                        {
                            Name = "Banknotes",
                        },
                        new SubCategory()
                        {
                            Name = "World Coins",
                        },
                    },
                },
                new Category()
                {
                    Name = "Fashion",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Bags",
                        },
                        new SubCategory()
                        {
                            Name = "Clothing",
                        },
                        new SubCategory()
                        {
                            Name = "Fashion Accessories",
                        },
                        new SubCategory()
                        {
                            Name = "Shoes",
                        },
                    },
                },
                new Category()
                {
                    Name = "Interiors & Decorations",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Antiques",
                        },
                        new SubCategory()
                        {
                            Name = "Clocks",
                        },
                        new SubCategory()
                        {
                            Name = "Cooking & Dining",
                        },
                        new SubCategory()
                        {
                            Name = "Decorative Objects",
                        },
                        new SubCategory()
                        {
                            Name = "Design & Vintage",
                        },
                        new SubCategory()
                        {
                            Name = "Plants & Bonsai",
                        },
                        new SubCategory()
                        {
                            Name = "Interiors",
                        },
                    },
                },
                new Category()
                {
                    Name = "Jewellery & Precious Stones",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Diamonds",
                        },
                        new SubCategory()
                        {
                            Name = "Gemstones",
                        },
                        new SubCategory()
                        {
                            Name = "Jewellery",
                        },
                    },
                },
                new Category()
                {
                    Name = "Military & Weaponry",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Military",
                        },
                        new SubCategory()
                        {
                            Name = "Weaponry",
                        },
                    },
                },
                new Category()
                {
                    Name = "Music & Cameras",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Audio Equipment",
                        },
                        new SubCategory()
                        {
                            Name = "Cameras & Optical Equipment",
                        },
                        new SubCategory()
                        {
                            Name = "Vinyl Records",
                        },
                        new SubCategory()
                        {
                            Name = "Musical Instruments",
                        },
                    },
                },
                new Category()
                {
                    Name = "Sports",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Bicycles",
                        },
                        new SubCategory()
                        {
                            Name = "Sports Equipment",
                        },
                    },
                },
                new Category()
                {
                    Name = "Toys & Models",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Model Cars",
                        },
                        new SubCategory()
                        {
                            Name = "Model Trains",
                        },
                        new SubCategory()
                        {
                            Name = "Model Planes",
                        },
                        new SubCategory()
                        {
                            Name = "Toys",
                        },
                    },
                },
                new Category()
                {
                    Name = "Watches & Accessories",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Luxury Accessories",
                        },
                        new SubCategory()
                        {
                            Name = "Watches",
                        },
                    },
                },
                new Category()
                {
                    Name = "Wine & Whisky",
                    SubCategories = new HashSet<SubCategory>
                    {
                        new SubCategory()
                        {
                            Name = "Champagne",
                        },
                        new SubCategory()
                        {
                            Name = "Port & Sweet Wines",
                        },
                        new SubCategory()
                        {
                            Name = "Whisky",
                        },
                        new SubCategory()
                        {
                            Name = "Wine",
                        },
                    },
                },
            };
        }
    }
}
