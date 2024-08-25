db.createUser(
    {
        user: "fizzbuzz",
        pwd: "fizzbuzz",
        roles: [
            {
                role: "readWrite",
                db: "fizzbuzz"
            }
        ]
    }
);
//db.createCollection("fizzbuzz"); //MongoDB creates the database when you first store data in that database
