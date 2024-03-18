build:
	@echo "Building..."
	@docker compose build

run:
	@echo "Running..."
	@docker compose up --build


migration:
	@echo "Creating migration..."
	@dotnet ef migrations add Init -p src/API -s src/API -o Data/Migrations