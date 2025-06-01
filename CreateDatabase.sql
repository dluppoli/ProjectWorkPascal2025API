USE master
DROP DATABASE IF EXISTS Restaurant
CREATE DATABASE Restaurant
GO
USE Restaurant

CREATE TABLE Tables(
	Id INT PRIMARY KEY,
	Occupied BIT NOT NULL DEFAULT 0,
	OccupancyDate DATETIME NULL,
	Occupants INT NULL,
	TableKey VARCHAR(64) NULL
)

INSERT INTO Tables(Id) VALUES (1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12),(13),(14),(15),(16),(17),(18),(19),(20)

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(1000) NOT NULL,
	Image VARCHAR(1000) NOT NULL,
	OrderIndex INT NOT NULL	
)

INSERT INTO Categories (Name, Image, OrderIndex) VALUES
('Menu', 'menu.jpg',0),
('Appetizers', 'appetizers.jpg',1),
('Kan BBQ', 'kanbbq.jpg',5),
('Noodles', 'noodles.jpg',4),
('Premium Carpaccio (5pcs)', 'premiumcarpaccio.jpg',13),
('Premium HandRolls', 'premiumhandrolls.jpg',8),
('Premium Rolls', 'premiumrolls.jpg',12),
('Premium Sashimi', 'premiumsashimi.jpg',10),
('Premium Sushi (2pcs)', 'premiumsushi.jpg',9),
('Premium Tapas', 'premiumtapas.jpg',11),
('Regular Rolls', 'regularrolls.jpg',6),
('Regular Sushi', 'regularsushi.jpg',7),
('Salads', 'salads.jpg',2),
('Tapas', 'tapas.jpg',3),
('Beverages', 'beverages.jpg',50),
('Drinks', 'drinks.jpg',99)


CREATE TABLE ProductPrepStations(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(1000) NOT NULL
)

INSERT INTO ProductPrepStations(Name) VALUES('Cucina'),('Bar')

CREATE TABLE Products(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Name VARCHAR(1000) NOT NULL,
	Description VARCHAR(MAX) NOT NULL,
	Price FLOAT NOT NULL DEFAULT 0,
	Image VARCHAR(1000) NOT NULL,
	IdCategory INT NOT NULL,
	IdPostazionePreparazione INT NOT NULL DEFAULT 1,
	FOREIGN KEY (IdCategory) REFERENCES Categories(Id),
	FOREIGN KEY (IdPostazionePreparazione) REFERENCES ProductPrepStations(Id)
)

CREATE TABLE Orders(
	Id INT PRIMARY KEY IDENTITY(1,1),
	TableId INT NOT NULL,
	TableKey VARCHAR(64) NOT NULL,
	ProductId INT NOT NULL,
	Qty INT NOT NULL,
	Price FLOAT NOT NULL,
	OrderDate DATETIME NOT NULL,
	CompletionDate DATETIME,
	FOREIGN KEY (ProductId) REFERENCES Products(Id)
)
INSERT INTO Products (Name, Description, IdCategory, Image,Price) VALUES ('Menù All You Can Eat', '', 1, '',24.90) 
INSERT INTO Products (Name, Description, IdCategory, Image, Price) VALUES 
('Jalapeno Bomb', '',2, 'jalapeno_bomb.jpeg',0), 
('Miso Soup', '',2, 'miso_soup.jpeg',0), 
('Garlic Butter Edamame', '',2, 'garlic_butter_edamame.jpeg',0), 
('Takoyaki', '',2, 'takoyaki.jpeg',0), 
('Corn Cheese', '',2, 'corn_cheese.jpeg',0), 
('Truffle Fries', '',2, 'truffle_fries.jpeg',0), 
('Sea Salt Edamame', '',2, 'sea_salt_edamame.jpeg',0), 
('Okonomi Fries', '',2, 'okonomi_fries.jpeg',0), 
('Cucumber Salad', '',13, 'cucumber_salad.jpeg',0), 
('Micro Spring Salad', '',13, 'micro_spring_salad.jpeg',0), 
('Seaweed Salad', '',13, 'seaweed_salad.jpeg',0), 
('Salmon Skin Salad', '',13, 'salmon_skin_salad.jpeg',0), 
('Deep Fried Squid (3-5 pcs)', '3-5 Pieces ',14, 'deep_fried_squid.jpeg',0), 
('Fried Oyster (5pcs)', '3 Pieces ',14, 'fried_oyster.jpeg',0), 
('Baked Green Mussels (2pcs)', '2 Pieces ',14, 'baked_green_mussels.jpeg',0), 
('Cajun Shrimp Udon Pasta', '',4, 'cajun_shrimp_udon_pasta.jpeg',0), 
('Garlic Chicken', '',3, 'garlic_chicken.jpeg',0), 
('Hawaiian Steak', 'Item comes fully cooked. ',3, 'hawaiian_steak.jpeg',0), 
('Galbi', '',3, 'galbi.jpeg',0), 
('Spicy Pork Bulgogi', '',3, 'spicy_pork_bulgogi.jpeg',0), 
('Bacon Pork Belly', '',3, 'bacon_pork_belly.jpeg',0), 
('Beef Belly', 'Item comes fully cooked. ',3, 'beef_belly.jpeg',0), 
('Beef Bulgogi', '',3, 'beef_bulgogi.jpeg',0), 
('Dragon Roll', '8 Pieces - In: Crab Meat, Shrimp Tempura, Avocado  - Out: Eel, Avocado, Katsuobushi, Eel Sauce ',11, 'dragon_roll.jpeg',0), 
('Bonnie & Clyde', '8 Pieces  - In: Spicy Tuna, Cucumber  - Out: Salmon, Tuna, Lemon, Jalapeno, Spicy Mayo, Cheese Powder, Sriracha, Garlic Mayo, Mozzarella Cheese ',11, 'bonnie_clyde.jpeg',0), 
('Spicy Tuna Shrimp Roll', '8 Pieces - In: Shrimp Tempura, Avocado, Crab Meat  - Out: Spicy Tuna, Mozzarella Cheese, Garlic Mayo, Spicy Mayo, Eel Sauce, Green Onion, Cheese Powder ',11, 'spicy_tuna_shrimp_roll.jpeg',0), 
('Tiger Roll', '',11, 'tiger_roll.jpeg',0), 
('After Rain Roll', '8 Pieces - In: Crab Meat, Avocado, Cucumber  - Out: Salmon, Tuna, Albacore, Shrimp, Tilapia, Avocado, Soy Mustard ',11, 'after_rain_roll.jpeg',0), 
('California Roll', '8 Pieces - In: Crab Meat, Avocado, ',11, 'california_roll.jpeg',0), 
('Baked Lobster Roll', '4 Pieces - In: Crab Meat, Avocado, Cucumber  - Out: Lobster, Mozzarella Cheese, Spicy Mayo, Eel Sauce, Green Onion, Masago ',11, 'baked_lobster_roll.jpeg',0), 
('Philadelphia Roll', '8 Pieces - In: Salmon, Avocado, Cream Cheese ',11, 'philadelphia_roll.jpeg',0), 
('Salmon Skin Roll', '5 Pieces - In: Salmon Skin, Cucumber, Gobo ',11, 'salmon_skin_roll.jpeg',0), 
('Spicy Tuna Roll', '8 Pieces - In: Spicy Tuna, Cucumber ',11, 'spicy_tuna_roll.jpeg',0), 
('Shooting Star', '',11, 'shooting_star.jpeg',0), 
('Roly Poly', '4 Pieces - In: Crab Meat  - Out: Salmon, Masago, Green Onion, Soy Mustard, Eel Sauce ',11, 'roly_poly.jpeg',0), 
('Tunapeño', '4 Pieces - In: Spicy Tuna  - Out: Tuna Tataki, Jalapeno, Spicy Mayo, Sriracha, Soy Mustard ',11, 'tunapeno.jpeg',0), 
('Creamy S & S', '4 Pieces - In: Cream Cheese,  Shrimp Tempura - Out: Smoked Salmon, Eel Sauce, Sweet Chili, Green Onion ',11, 'creamy_s_s.jpeg',0), 
('Salmon To The Moon', '4 Pieces - In: Cream Cheese - Out: Salmon, Avocado, Green Onion, Masago, Soy Mustard, Lemon ',11, 'salmon_to_the_moon.jpeg',0), 
('Hot Cheetos Roll', '8 Pieces - In: Shrimp Tempura, Avocado, Cream Cheese - Out: Cheetos, Eel Sauce, Spicy Mayo, Wasabi Mayo ',11, 'hot_cheetos_roll.jpeg',0), 
('Fried Onion Albacore', '',11, 'fried_onion_albacore.jpeg',0), 
('Hot & Fresh Roll', '8pcs ',11, 'hot_fresh_roll.jpeg',0), 
('Aloha Fire', '8pcs ',11, 'aloha_fire.jpeg',0), 
('Emergency Roll', '8pcs ',11, 'emergency_roll.jpeg',0), 
('Yellowtail Jalapeno', '8 Pieces - In: Spicy tuna, Fried jalapeno - Out: Torched yellowtail, Jalapeno, Sriracha ',11, 'yellowtail_jalapeno.jpeg',0), 
('Tuna', '2 Pieces ',12, 'tuna.jpeg',0), 
('Snowflake Salmon', '2 Pieces ',12, 'snowflake_salmon.jpeg',0), 
('Salmon', '2 Pieces ',12, 'salmon.jpeg',0), 
('Albacore', '2 Pieces ',12, 'albacore.jpeg',0), 
('Peppered Salmon', '2 Pieces ',12, 'peppered_salmon.jpeg',0), 
('Yellowtail', '2 Pieces ',12, 'yellowtail.jpeg',0), 
('Tamago', '2 Pieces ',12, 'tamago.jpeg',0), 
('Tuna Tadaki', '2 Pieces ',12, 'tuna_tadaki.jpeg',0), 
('Seared Tuna', '2 Pieces ',12, 'seared_tuna.jpeg',0), 
('Shrimp', '2 Pieces ',12, 'shrimp.jpeg',0), 
('Crab Stick', '2 Pieces ',12, 'crab_stick.jpeg',0), 
('Freshwater Eel', '2 Pieces ',12, 'freshwater_eel.jpeg',0), 
('Escolar', '2 Pieces ',12, 'escolar.jpeg',0), 
('Ninja Tuna', '2 Pieces ',12, 'ninja_tuna.jpeg',0), 
('Inari Original', '',12, 'inari_original.jpeg',0), 
('Inari Crabmeat', '',12, 'inari_crabmeat.jpeg',0), 
('Inari Bulgogi', '',12, 'inari_bulgogi.jpeg',0), 
('Inari Eel', '',12, 'inari_eel.jpeg',0), 
('Inari Spicy Tuna', '',12, 'inari_spicy_tuna.jpeg',0), 
('Yellowtail', '',6, 'yellowtail.jpeg',1), 
('Salmon', '',6, 'salmon.jpeg',1), 
('Tuna', '',6, 'tuna.jpeg',1), 
('Truffle Salmon', '2 Pieces ',9, 'truffle_salmon.jpeg',1), 
('Premium Tuna', '',9, 'premium_tuna.jpeg',1), 
('Scallop', '',9, 'scallop.jpeg',1), 
('Seared yellowtail (3pcs)', '',8, 'seared_yellowtail.jpeg',1), 
('Yellowtail (3pcs)', '',8, 'yellowtail.jpeg',1), 
('Premium Salmon (3pcs)', '',8, 'premium_salmon.jpeg',1), 
('Premium Tuna (3pcs)', '',8, 'premium_tuna.jpeg',1), 
('Yellowtail', '',5, 'yellowtail.jpeg',1), 
('Tuna', '',5, 'tuna.jpeg',1), 
('Salmon', '',5, 'salmon.jpeg',1), 
('Seared Tuna', '',5, 'seared_tuna.jpeg',1), 
('Seared Salmon', '',5, 'seared_salmon.jpeg',1), 
('Albacore', '',5, 'albacore.jpeg',1), 
('Hamachi Kama', '',10, 'hamachi_kama_limited.jpeg',1), 
('Salmon Kama', '',10, 'salmon_kama.jpeg',1), 
('Fried Soft Shell Crab (2pcs)', '',10, 'fried_soft_shell_crab.jpeg',1), 
('Yellowpeno (No Rice Roll - 4pcs)', 'In: Negitoro - Out: Yellowtail, Fresh Jalapeno, Sriracha, Micro Green, Yuzu Kosho Sauce, Black Pepper ',7, 'yellowpeno.jpeg',1), 
('Softshell Crab Roll (5pcs)', 'In: Carrots, Spring Mix, Cucumber, Fried Softshell crab - Out: Eel Sauce, Sesame seed ',7, 'softshell_crab_roll.jpeg',1);


INSERT INTO Products (Name, Description, IdCategory, Image,Price) VALUES ('Water','',16,'water.jpg' ,2),
('Coke','',16,'coke.jpg' ,3),('Lemon Tea','',16,'lemontea.jpeg' ,3),
('Chocolate 3 layer cake','',16,'ChocolateCake.jpeg' ,5),('Lime Pie','',16,'limepie.jpeg' ,5),('Sea Salt Caramel Cheesecake','',16,'caramelcheesecake.jpeg' ,5)

CREATE TABLE Users(
	Id INT PRIMARY KEY IDENTITY(1,1),
	Username NVARCHAR(1000) NOT NULL,
	Password VARCHAR(MAX) NOT NULL,
	Salt CHAR(32) NOT NULL,
	LastLogin DATETIME,
	LastLogout DATETIME,
	SessionToken VARCHAR(128)
)
CREATE TABLE RevokedTokens(
	Token VARCHAR(128) PRIMARY KEY,
	Expire DATETIME
)

INSERT INTO Users(Username, Password, Salt) VALUES('test','7b049b3b040ef5748c327c24ff1cad329f228d0a5c1c47c28416a70d7019cbf8165b76af309ff07e067de730b7079f18e4630c55e826782437a9f38a8e6bc75a','4358f1c8d48b15a7ea42ecb9138dcaad')