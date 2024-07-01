CREATE TABLE
    IF NOT EXISTS products (
        id UUID PRIMARY KEY,
        name TEXT NOT NULL,
        category TEXT NOT NULL,
        sub_category TEXT NOT NULL
    );

CREATE TABLE
    IF NOT EXISTS reviews (
        id UUID PRIMARY KEY,
        product_id UUID NOT NULL,
        rating INT NOT NULL,
        text TEXT NOT NULL,
        CONSTRAINT fk_product FOREIGN KEY (product_id) REFERENCES products (id) ON DELETE CASCADE
    );

CREATE INDEX IF NOT EXISTS idx_reviews_product_id ON reviews (product_id);