ALTER TABLE Posts
ADD CONSTRAINT FK_Posts_Landlords_LandlordId
FOREIGN KEY (Landlord_id) REFERENCES Users(id);
