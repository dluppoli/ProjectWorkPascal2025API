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
('Menu', 'Menu.jpg',0),
('Appetizers', 'Appetizers.jpg',1),
('Kan BBQ', 'KanBBQ.jpg',5),
('Noodles', 'Noodles.jpg',4),
('Premium Carpaccio (5pcs)', 'PremiumCarpaccio.jpg',13),
('Premium HandRolls', 'PremiumHandRolls.jpg',8),
('Premium Rolls', 'PremiumRolls.jpg',12),
('Premium Sashimi', 'PremiumSashimi.jpg',10),
('Premium Sushi (2pcs)', 'PremiumSushi.jpg',9),
('Premium Tapas', 'PremiumTapas.jpg',11),
('Regular Rolls', 'RegularRolls.jpg',6),
('Regular Sushi', 'RegularSushi.jpg',7),
('Salads', 'Salads.jpg',2),
('Tapas', 'Tapas.jpg',3);

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
INSERT INTO Products (Name, Description, IdCategory, Image,Price) VALUES 
('Jalapeno Bomb', '',2, 'Jalapeno_Bomb.jpeg',0), 
('Miso Soup', '',2, 'Miso_Soup.jpeg',0), 
('Garlic Butter Edamame', '',2, 'Garlic_Butter_Edamame.jpeg',0), 
('Takoyaki', '',2, 'Takoyaki.jpeg',0), 
('Corn Cheese', '',2, 'Corn_Cheese.jpeg',0), 
('Truffle Fries', '',2, 'Truffle_Fries.jpeg',0), 
('Sea Salt Edamame', '',2, 'Sea_Salt_Edamame.jpeg',0), 
('Okonomi Fries', '',2, 'Okonomi_Fries.jpeg',0), 
('Cucumber Salad', '',13, 'Cucumber_Salad.jpeg',0), 
('Micro Spring Salad', '',13, 'Micro_Spring_Salad.jpeg',0), 
('Seaweed Salad', '',13, 'Seaweed_Salad.jpeg',0), 
('Salmon Skin Salad', '',13, 'Salmon_Skin_Salad.jpeg',0), 
('Deep Fried Squid (3-5 pcs)', '3-5 Pieces ',14, 'Deep_Fried_Squid.jpeg',0), 
('Fried Oyster (5pcs)', '3 Pieces ',14, 'Fried_Oyster.jpeg',0), 
('Baked Green Mussels (2pcs)', '2 Pieces ',14, 'Baked_Green_Mussels.jpeg',0), 
('Cajun Shrimp Udon Pasta', '',4, 'Cajun_Shrimp_Udon_Pasta.jpeg',0), 
('Garlic Chicken', '',3, 'Garlic_Chicken.jpeg',0), 
('Hawaiian Steak', 'Item comes fully cooked. ',3, 'Hawaiian_Steak.jpeg',0), 
('Galbi', '',3, 'Galbi.jpeg',0), 
('Spicy Pork Bulgogi', '',3, 'Spicy_Pork_Bulgogi.jpeg',0), 
('Bacon Pork Belly', '',3, 'Bacon_Pork_Belly.jpeg',0), 
('Beef Belly', 'Item comes fully cooked. ',3, 'Beef_Belly.jpeg',0), 
('Beef Bulgogi', '',3, 'Beef_Bulgogi.jpeg',0), 
('Dragon Roll', '8 Pieces - In: Crab Meat, Shrimp Tempura, Avocado  - Out: Eel, Avocado, Katsuobushi, Eel Sauce ',11, 'Dragon_Roll.jpeg',0), 
('Bonnie &amp; Clyde', '8 Pieces  - In: Spicy Tuna, Cucumber  - Out: Salmon, Tuna, Lemon, Jalapeno, Spicy Mayo, Cheese Powder, Sriracha, Garlic Mayo, Mozzarella Cheese ',11, 'Bonnie_&amp;_Clyde.jpeg',0), 
('Spicy Tuna Shrimp Roll', '8 Pieces - In: Shrimp Tempura, Avocado, Crab Meat  - Out: Spicy Tuna, Mozzarella Cheese, Garlic Mayo, Spicy Mayo, Eel Sauce, Green Onion, Cheese Powder ',11, 'Spicy_Tuna_Shrimp_Roll.jpeg',0), 
('Tiger Roll', '',11, 'Tiger_Roll.jpeg',0), 
('After Rain Roll', '8 Pieces - In: Crab Meat, Avocado, Cucumber  - Out: Salmon, Tuna, Albacore, Shrimp, Tilapia, Avocado, Soy Mustard ',11, 'After_Rain_Roll.jpeg',0), 
('California Roll', '8 Pieces - In: Crab Meat, Avocado, ',11, 'California_Roll.jpeg',0), 
('Baked Lobster Roll', '4 Pieces - In: Crab Meat, Avocado, Cucumber  - Out: Lobster, Mozzarella Cheese, Spicy Mayo, Eel Sauce, Green Onion, Masago ',11, 'Baked_Lobster_Roll.jpeg',0), 
('Philadelphia Roll', '8 Pieces - In: Salmon, Avocado, Cream Cheese ',11, 'Philadelphia_Roll.jpeg',0), 
('Salmon Skin Roll', '5 Pieces - In: Salmon Skin, Cucumber, Gobo ',11, 'Salmon_Skin_Roll.jpeg',0), 
('Spicy Tuna Roll', '8 Pieces - In: Spicy Tuna, Cucumber ',11, 'Spicy_Tuna_Roll.jpeg',0), 
('Shooting Star', '',11, 'Shooting_Star.jpeg',0), 
('Roly Poly', '4 Pieces - In: Crab Meat  - Out: Salmon, Masago, Green Onion, Soy Mustard, Eel Sauce ',11, 'Roly_Poly.jpeg',0), 
('Tunapeño', '4 Pieces - In: Spicy Tuna  - Out: Tuna Tataki, Jalapeno, Spicy Mayo, Sriracha, Soy Mustard ',11, 'Tunapeño.jpeg',0), 
('Creamy S&amp;S', '4 Pieces - In: Cream Cheese,  Shrimp Tempura - Out: Smoked Salmon, Eel Sauce, Sweet Chili, Green Onion ',11, 'Creamy_S&amp;S.jpeg',0), 
('Salmon To The Moon', '4 Pieces - In: Cream Cheese - Out: Salmon, Avocado, Green Onion, Masago, Soy Mustard, Lemon ',11, 'Salmon_To_The_Moon.jpeg',0), 
('Hot Cheetos Roll', '8 Pieces - In: Shrimp Tempura, Avocado, Cream Cheese - Out: Cheetos, Eel Sauce, Spicy Mayo, Wasabi Mayo ',11, 'Hot_Cheetos_Roll.jpeg',0), 
('Fried Onion Albacore', '',11, 'Fried_Onion_Albacore.jpeg',0), 
('Hot &amp; Fresh Roll', '8pcs ',11, 'Hot_&amp;_Fresh_Roll.jpeg',0), 
('Aloha Fire', '8pcs ',11, 'Aloha_Fire.jpeg',0), 
('Emergency Roll', '8pcs ',11, 'Emergency_Roll.jpeg',0), 
('Yellowtail Jalapeno', '8 Pieces - In: Spicy tuna, Fried jalapeno - Out: Torched yellowtail, Jalapeno, Sriracha ',11, 'Yellowtail_Jalapeno.jpeg',0), 
('Tuna', '2 Pieces ',12, 'Tuna.jpeg',0), 
('Snowflake Salmon', '2 Pieces ',12, 'Snowflake_Salmon.jpeg',0), 
('Salmon', '2 Pieces ',12, 'Salmon.jpeg',0), 
('Albacore', '2 Pieces ',12, 'Albacore.jpeg',0), 
('Peppered Salmon', '2 Pieces ',12, 'Peppered_Salmon.jpeg',0), 
('Yellowtail', '2 Pieces ',12, 'Yellowtail.jpeg',0), 
('Tamago', '2 Pieces ',12, 'Tamago.jpeg',0), 
('Tuna Tadaki', '2 Pieces ',12, 'Tuna_Tadaki.jpeg',0), 
('Seared Tuna', '2 Pieces ',12, 'Seared_Tuna.jpeg',0), 
('Shrimp', '2 Pieces ',12, 'Shrimp.jpeg',0), 
('Crab Stick', '2 Pieces ',12, 'Crab_Stick.jpeg',0), 
('Freshwater Eel', '2 Pieces ',12, 'Freshwater_Eel.jpeg',0), 
('Escolar', '2 Pieces ',12, 'Escolar.jpeg',0), 
('Ninja Tuna', '2 Pieces ',12, 'Ninja_Tuna.jpeg',0), 
('Inari Original', '',12, 'Inari_Original.jpeg',0), 
('Inari Crabmeat', '',12, 'Inari_Crabmeat.jpeg',0), 
('Inari Bulgogi', '',12, 'Inari_Bulgogi.jpeg',0), 
('Inari Eel', '',12, 'Inari_Eel.jpeg',0), 
('Inari Spicy Tuna', '',12, 'Inari_Spicy_Tuna.jpeg',0), 
('Yellowtail', '',6, 'Yellowtail.jpeg',1), 
('Salmon', '',6, 'Salmon.jpeg',1), 
('Tuna', '',6, 'Tuna.jpeg',1), 
('Truffle Salmon', '2 Pieces ',9, 'Truffle_Salmon.jpeg',1), 
('Premium Tuna', '',9, 'Premium_Tuna.jpeg',1), 
('Scallop', '',9, 'Scallop.jpeg',1), 
('Seared yellowtail (3pcs)', '',8, 'Seared_yellowtail.jpeg',1), 
('Yellowtail (3pcs)', '',8, 'Yellowtail.jpeg',1), 
('Premium Salmon (3pcs)', '',8, 'Premium_Salmon.jpeg',1), 
('Premium Tuna (3pcs)', '',8, 'Premium_Tuna.jpeg',1), 
('Yellowtail', '',5, 'Yellowtail.jpeg',1), 
('Tuna', '',5, 'Tuna.jpeg',1), 
('Salmon', '',5, 'Salmon.jpeg',1), 
('Seared Tuna', '',5, 'Seared_Tuna.jpeg',1), 
('Seared Salmon', '',5, 'Seared_Salmon.jpeg',1), 
('Albacore', '',5, 'Albacore.jpeg',1), 
('Hamachi Kama', '',10, 'hamachi_kama_limited.jpeg',1), 
('Salmon Kama', '',10, 'Salmon_Kama.jpeg',1), 
('Fried Soft Shell Crab (2pcs)', '',10, 'Fried_Soft_Shell_Crab.jpeg',1), 
('Yellowpeno (No Rice Roll - 4pcs)', 'In: Negitoro - Out: Yellowtail, Fresh Jalapeno, Sriracha, Micro Green, Yuzu Kosho Sauce, Black Pepper ',7, 'Yellowpeno.jpeg',1), 
('Softshell Crab Roll (5pcs)', 'In: Carrots, Spring Mix, Cucumber, Fried Softshell crab - Out: Eel Sauce, Sesame seed ',7, 'Softshell_Crab_Roll.jpeg',1);


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