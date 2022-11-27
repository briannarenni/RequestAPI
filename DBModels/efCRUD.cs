// * Example CRUD with EF Core

// ! Query data
// var pizzas = await db.Pizzas.ToListAsync();

// ! Insert data
// await db.pizzas.AddAsync(
//     new Pizza { ID = 1, Name = "Pepperoni", Description = "The classic pepperoni pizza" });

// ! Update data
// int id = 1;
// var updatepizza = new Pizza { Name = "Pineapple", Description = "Ummmm?" };
// var pizza = await db.pizzas.FindAsync(id);
// if (pizza is null)
// {
     //Handle error
// }
// pizza.Item = updatepizza.Item;
// pizza.IsComplete = updatepizza.IsComplete;
// await db.SaveChangesAsync();

// ! Delete data
// var pizza = await db.pizzas.FindAsync(id);
// if (pizza is null)
// {
     //Handle error
// }
// db.pizzas.Remove(pizza);