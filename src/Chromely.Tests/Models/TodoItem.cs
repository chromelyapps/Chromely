
namespace Chromely.Tests.Models;

// Copyright © 2017 Chromely Projects. All rights reserved.
// Use of this source code is governed by MIT license that can be found in the LICENSE file.

public class TodoItem
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    public static string FakeTodoItemName
    {
        get
        {
            var faker = new Faker("en");
            return faker.Random.Word();
        }
    }

    public static TodoItem FakeTodoItem
    {
        get
        {
            var todoItemFaker = new Faker<TodoItem>()
                .RuleFor(o => o.Id, f => f.Random.Number(10000, 99999))
                .RuleFor(o => o.Name, f => f.Random.Word())
                .RuleFor(o => o.IsComplete, f => f.Random.Bool());

            return todoItemFaker.Generate(1)[0];
        }
    }
}