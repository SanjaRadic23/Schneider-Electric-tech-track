## DataSet
CREATE TABLE Users (
    id_user INTEGER PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    phone_number VARCHAR(15) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    role VARCHAR(30) NOT NULL
);

CREATE TABLE Suppliers (
    id_supplier INTEGER PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    phone_number VARCHAR(15) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    address VARCHAR(255) NOT NULL
);

CREATE TABLE TechProducts (
    id_product INTEGER PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(255),
    quantity INTEGER NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    supplier_id INTEGER,
    FOREIGN KEY (supplier_id) REFERENCES Suppliers(id_supplier)
);

CREATE TABLE UsersOrders (
    id_order INTEGER PRIMARY KEY,
    creation_date DATE NOT NULL,
    status VARCHAR(20) CHECK (status IN ('created', 'sent', 'delivered')) NOT NULL,
    user_id INTEGER,
    FOREIGN KEY (user_id) REFERENCES Users(id_user)
);

CREATE TABLE PurchasesOrders (
    id_order INTEGER PRIMARY KEY,
    creation_date DATE NOT NULL,
    status VARCHAR(20) CHECK (status IN ('created', 'sent', 'delivered')) NOT NULL,
    user_id INTEGER,
    FOREIGN KEY (user_id) REFERENCES Users(id_user)
);

CREATE TABLE UsersOrderItems(
    user_order_id INTEGER,
    product_id INTEGER,
    quantity INTEGER NOT NULL,
    total_price DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY (user_order_id, product_id),
    FOREIGN KEY (user_order_id) REFERENCES UsersOrders(id_order),
    FOREIGN KEY (product_id) REFERENCES TechProducts(id_product)
);


CREATE TABLE PurchasesOrderItems(
    purchase_order_id INTEGER,
    product_id INTEGER,
    quantity INTEGER NOT NULL,
    total_price DECIMAL(10, 2) NOT NULL,
    PRIMARY KEY (purchase_order_id, product_id),
    FOREIGN KEY (purchase_order_id) REFERENCES PurchasesOrders(id_order),
    FOREIGN KEY (product_id) REFERENCES TechProducts(id_product)
);

CREATE TABLE UserAccounts (
    id_account INTEGER PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    user_id INTEGER NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(id_user)
);

## PL/SQL func and triggers
CREATE OR REPLACE FUNCTION GetMonthlyOrderStatistics
RETURN SYS_REFCURSOR
IS
    result_cursor SYS_REFCURSOR;
BEGIN
    OPEN result_cursor FOR
        SELECT 
            TO_CHAR(uo.creation_date, 'YYYY-MM') AS month,
            COUNT(uo.id_order) AS total_orders,
            SUM(uoi.total_price) AS total_revenue,
            AVG(uoi.total_price) AS average_order_value
        FROM 
            UsersOrders uo
        JOIN 
            UsersOrderItems uoi ON uo.id_order = uoi.user_order_id
        GROUP BY 
            TO_CHAR(uo.creation_date, 'YYYY-MM')
        ORDER BY 
            month;

    RETURN result_cursor;
END GetMonthlyOrderStatistics;

CREATE OR REPLACE FUNCTION GetQuarterlyOrderStatistics
RETURN SYS_REFCURSOR
IS
    result_cursor SYS_REFCURSOR;
BEGIN
    OPEN result_cursor FOR
        SELECT 
            TO_CHAR(uo.creation_date, 'YYYY-Q') AS quarter,
            COUNT(uo.id_order) AS total_orders,
            SUM(uoi.total_price) AS total_revenue,
            AVG(uoi.total_price) AS average_order_value
        FROM 
            UsersOrders uo
        JOIN 
            UsersOrderItems uoi ON uo.id_order = uoi.user_order_id
        GROUP BY 
            TO_CHAR(uo.creation_date, 'YYYY-Q')
        ORDER BY 
            quarter;

    RETURN result_cursor;
END GetQuarterlyOrderStatistics;

CREATE OR REPLACE FUNCTION GetStockAndTotalValue
RETURN SYS_REFCURSOR
IS
    result_cursor SYS_REFCURSOR;
BEGIN
    OPEN result_cursor FOR
        SELECT 
            name AS product_name,
            quantity AS stock_quantity,
            quantity * price AS total_value
        FROM 
            TechProducts;

    RETURN result_cursor;
END GetStockAndTotalValue;

CREATE OR REPLACE FUNCTION GetTopSoldProductsLast7Days
RETURN SYS_REFCURSOR
IS
    result_cursor SYS_REFCURSOR;
BEGIN
    OPEN result_cursor FOR
        SELECT 
            tp.name AS product_name,
            SUM(uoi.quantity) AS total_sold
        FROM 
            UsersOrderItems uoi
        JOIN 
            TechProducts tp ON uoi.product_id = tp.id_product
        JOIN 
            UsersOrders uo ON uoi.user_order_id = uo.id_order
        WHERE 
            uo.creation_date >= SYSDATE - INTERVAL '7' DAY
        GROUP BY 
            tp.name
        ORDER BY 
            total_sold DESC
        FETCH FIRST 5 ROWS ONLY;

    RETURN result_cursor;
END GetTopSoldProductsLast7Days;

CREATE OR REPLACE FUNCTION GetYearlyOrderStatistics
RETURN SYS_REFCURSOR
IS
    result_cursor SYS_REFCURSOR;
BEGIN
    OPEN result_cursor FOR
        SELECT 
            TO_CHAR(uo.creation_date, 'YYYY') AS year,
            COUNT(uo.id_order) AS total_orders,
            SUM(uoi.total_price) AS total_revenue,
            AVG(uoi.total_price) AS average_order_value
        FROM 
            UsersOrders uo
        JOIN 
            UsersOrderItems uoi ON uo.id_order = uoi.user_order_id
        GROUP BY 
            TO_CHAR(uo.creation_date, 'YYYY')
        ORDER BY 
            year;

    RETURN result_cursor;
END GetYearlyOrderStatistics;

create or replace NONEDITIONABLE TRIGGER UpdateProductStock
AFTER INSERT ON UsersOrderItems
FOR EACH ROW
DECLARE
    v_current_quantity INT;
BEGIN
    SELECT quantity INTO v_current_quantity
    FROM TechProducts
    WHERE id_product = :NEW.product_id;

    IF v_current_quantity < :NEW.quantity THEN
        RAISE_APPLICATION_ERROR(-20001, 'Not enough stock for product ID: ' || :NEW.product_id);
    ELSE
        UPDATE TechProducts
        SET quantity = quantity - :NEW.quantity
        WHERE id_product = :NEW.product_id;
    END IF;
END;
create or replace NONEDITIONABLE TRIGGER UpdateProductStockOnPurchase
AFTER INSERT ON PurchasesOrderItems
FOR EACH ROW
DECLARE
    v_current_quantity INT;
BEGIN
    SELECT quantity INTO v_current_quantity
    FROM TechProducts
    WHERE id_product = :NEW.product_id;

    UPDATE TechProducts
    SET quantity = v_current_quantity + :NEW.quantity
    WHERE id_product = :NEW.product_id;
END;






