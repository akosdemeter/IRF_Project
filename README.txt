A sportszimulációs szoftver tervezett funkció:

- Adatbázis táblából adatok beolvasása (Csapatok neve, támadási és védekezési erősségi szintjük)
- Az alkalmazás a szükséges adattárolási és dekralálási funkciók után sportmérkőzés szimulációt hajt végre.
- Minden csapat kétszer játszik mindekivel, és az adott meccs eredménye alapján 3, 1 vagy 0 pontot kap.
- Ezek alapján kiderül, hogy melyik csapat milyen helyen végzett a bajnokságban, 
és az adatok kiírásra kerülnek egy csv fájlba.
- Egy mérkőzés a következő képpen zajlik:
	- Mindkét csapatnak 10-10 lehetősége van.
	- Először a középpálya és a védelem erőssége kerül összevetésre (véletlen faktorral kiegészítve), 
	amelyből kiderül, hogy az adot csapat a 10 lehetőségből hány helyzetet tudott kialakítani.
	- Ez után a támadási és kapus erősség kerül összevetésre (véletlen faktorral kiegészítve),
	amelyből kiderül, hogy az adott csapat a kialakított helyzettek közül hányat tudott értékesíteni.
	- Így megkapjuk, hogy melyik csapat hány gólt szerzett és mi lett a mérkőzés végeredménye.