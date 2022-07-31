CREATE TYPE Message AS TABLE( 
	id INT NOT NULL,
	text NVARCHAR(MAX),
	time DATETIME,
	userSend INT,
	userTake INT,
	PRIMARY KEY (id)
);