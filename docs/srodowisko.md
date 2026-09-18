# Środowisko testowe — dane do uzupełnienia przez opiekuna

Ten plik jest kontraktem między opiekunem a praktykantem. Bez wypełnionych
sekcji ekrany 2, 3 i 4 nie da się zaimplementować na rzeczywistych danych.

**Nie wpisuj tu haseł.** Hasła przekazujemy osobnym kanałem.

---

## 1. Testowa baza SQL (tylko do odczytu)

| Parametr | Wartość |
|---|---|
| Serwer / instancja | _do uzupełnienia_ |
| Baza danych | _do uzupełnienia_ |
| Uwierzytelnianie | Windows / SQL Server — _do uzupełnienia_ |
| Login (jeśli SQL Server) | _do uzupełnienia_ |
| Hasło | przekazane osobno, nie zapisujemy w repozytorium |
| Uprawnienia | wyłącznie `SELECT` |

## 2. Tabela kolektorów

Potrzebne do ekranu 2 (`SqlDataService.GetCollectorsAsync`).

| Znaczenie | Tabela.kolumna |
|---|---|
| Identyfikator kolektora | _do uzupełnienia_ |
| Nazwa opisowa | _do uzupełnienia_ |
| Numer seryjny (SN) | _do uzupełnienia_ |
| Adres IP | _do uzupełnienia_ |
| Czas ostatniego pomiaru | _do uzupełnienia_ |

Pytania do rozstrzygnięcia:

- Czy czas ostatniego pomiaru jest kolumną w tabeli kolektorów, czy trzeba go
  wyliczyć z tabeli pomiarów (`MAX(czas)`)?
- Jaki próg wieku danych uznajemy za „dane nieaktualne”? Zależy od
  częstotliwości zapisu pomiarów.

## 3. Pomiary GIP

Potrzebne do ekranu 3 (`SqlDataService.GetGipSeriesAsync`).
Zakres obejmuje **GIP2** — wszystkie wejścia jednocześnie.

| Znaczenie | Tabela.kolumna |
|---|---|
| Powiązanie z kolektorem | _do uzupełnienia_ |
| Numer wejścia GIP | _do uzupełnienia_ |
| Znacznik czasu | _do uzupełnienia_ |
| Wartość (0/1) | _do uzupełnienia_ |

Pytania do rozstrzygnięcia:

- Strefa czasowa znacznika: czas lokalny czy UTC?
- Ile wejść GIP ma jeden kolektor i czy liczba jest stała?
- Czy w bazie występują duplikaty próbek dla tego samego czasu?

## 4. Pomiary energii

Potrzebne do ekranu 3 (`SqlDataService.GetEnergySeriesAsync`).

| Znaczenie | Tabela.kolumna | Jednostka |
|---|---|---|
| Moc chwilowa | _do uzupełnienia_ | kW |
| Licznik energii | _do uzupełnienia_ | kWh |
| Znacznik czasu | _do uzupełnienia_ | — |

Pytania do rozstrzygnięcia:

- Czy moc jest zapisywana bezpośrednio, czy wyliczana z przyrostu licznika?
- Czy licznik energii może się zerować (wymiana urządzenia, przepełnienie)?

## 5. API sterowania wyjściami

Potrzebne do ekranu 4 (`CollectorApiClient.SetOutputAsync`).

| Parametr | Wartość |
|---|---|
| Dokumentacja API | _link lub ścieżka do pliku — do uzupełnienia_ |
| Adres bazowy | _do uzupełnienia_ |
| Metoda i ścieżka ustawienia wyjścia | _do uzupełnienia_ |
| Format żądania | _do uzupełnienia_ |
| Format odpowiedzi | _do uzupełnienia_ |
| Uwierzytelnianie | _do uzupełnienia_ |
| Endpoint odczytu stanu wyjść | _do uzupełnienia, jeśli istnieje_ |

Przykład poprawnego wywołania:

```
_do uzupełnienia — konkretne, działające żądanie_
```

### Numeracja wyjść

Numeracja musi pochodzić z dokumentacji urządzenia. Oznaczenia i kolory w mocku
są demonstracyjne i **nie są źródłem prawdy**.

| Numer wyjścia | Opis / przeznaczenie |
|---|---|
| _do uzupełnienia_ | _do uzupełnienia_ |

### Kolektor testowy

| Parametr | Wartość |
|---|---|
| Nazwa / SN | _do uzupełnienia_ |
| Adres IP | _do uzupełnienia_ |
| Podłączony do maszyn? | **nie** — warunek konieczny |

**Sterowanie wyjściami wolno testować wyłącznie na tym urządzeniu.**
Polecenia ON/OFF przełączają fizyczne wyjścia.

## 6. Zapytania kontrolne (dzień 2)

Praktykant zapisuje tu zapytania, na których potwierdził znaczenie kolumn.

```sql
-- lista kolektorów
-- do uzupełnienia przez praktykanta

-- przykładowe próbki GIP jednego kolektora
-- do uzupełnienia przez praktykanta

-- przykładowe próbki energii jednego kolektora
-- do uzupełnienia przez praktykanta
```
