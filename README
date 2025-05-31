# Project Work API - Documentazione compatta

Api a supporto di 4 differenti applicazioni:
- **Camerieri**
- **Clienti**
- **Cucina**
- **Statistiche**

---
## API di autenticazione (Non usate da app clienti)
### ```POST /login```
Autentica l'utente restituendo un JWT
```json
{
    "username":"test",
    "password": "test"
}
```
### ```POST /logout```
Chiude la sessione utente, invalidando il token


## API per applicazione cameriere
**NB** Tutte le API richiedono l'invio del token JWT

### ```GET /cameriere/tavoli```
Restituisce elenco dei tavoli
### ```GET /cameriere/tavoli/{id}```
Restituisce le informazioni di un tavolo dato il suo id (numerico)
### ```PUT /cameriere/tavoli/{id}```
Permette di aprire / chiudere un tavolo dato il suo id (numerico)
```json
//Request Body per apertura tavolo
{
    "id":3,
    "occupied": true,
    "occupants": 4
}

//Request Body per chiusura tavolo
{
    "id":3,
    "occupied": false,
}
```
### ```GET /cameriere/tavoli/{id}/conto```
Restituisce il conto del tavolo (identificato con il suo id numerico), composto dal totale e dal dettaglio di tutti gli ordini "a pagamento"
### ```GET /cameriere/tavoli/{id}/ordine```
Restituisce tutti i prodotti ordinati dal tavolo (identificato con il suo id numerico), inserendo anche quelli compresi nel menù all you can eat
### ```POST /cameriere/tavoli/{id}/ordine```
Aggiunge uno o più prodotti all'ordine del tavolo  (identificato con il suo id numerico)
```json
[
    {
        "productId": 76,
        "qty": 2
    }
]
```
### ```DELETE /cameriere/tavoli/{id}/ordine```
Cancella tutti i piatti ordinati da un tavolo (identificato con il suo id numerico)
### ```DELETE /cameriere/tavoli/{id}/ordine/{ido} (per cancellare l'ordine di un piatto)```
Cancella tutti un piatto (identificato con il suo id numerico) dall'ordine di un tavolo (identificato con il suo id numerico)
### ```GET /cameriere/prodotti```
Restituisce l'elenco di tutti i prodotti
### ```GET /cameriere/prodotti/{id}```
Restituisce il dettaglio di un prodotto, dato il suo id (numerico)
### ```GET /cameriere/categorie```
Restituisce l'elenco di tutte le categorie di prodotto
### ```GET /cameriere/categorie/{id}```
Restituisce il dettaglio di una categoria, dato il suo id (numerico)
### ```GET /cameriere/categorie/{id}/piatti```
Restituisce tutti i prodotti presenti in una categoria, dato il suo id (numerico)


## API per applicazione Cliente
**NB** Tutte le API richiedono l'invio dell'Api Key (Header APIKey)

### ```GET /cliente/prodotti```
Restituisce l'elenco di tutti i prodotti
### ```GET /cliente/prodotti/{id}```
Restituisce il dettaglio di un prodotto, dato il suo id (numerico)
### ```GET /cliente/categorie```
Restituisce l'elenco di tutte le categorie di prodotto
### ```GET /cliente/categorie/{id}```
Restituisce il dettaglio di una categoria, dato il suo id (numerico)
### ```GET /cliente/categorie/{id}/piatti```
Restituisce tutti i prodotti presenti in una categoria, dato il suo id (numerico)
### ```GET /cliente/preconto```
Restituisce il conto del tavolo, composto dal totale e dal dettaglio di tutti gli ordini "a pagamento"
### ```GET /cliente/ordine```
Restituisce tutti i prodotti ordinati dal tavolo, inserendo anche quelli compresi nel menù all you can ea
### ```POST /cliente/ordine```
Aggiunge uno o più prodotti all'ordine del tavolo
```json
[
    {
        "productId": 76,
        "qty": 2
    }
]
```

## API per applicazione Cucina
**NB** Tutte le API richiedono l'invio del token JWT

### ```GET /cucina/```
Restituisce tutte le stazioni di lavoro
### ```GET /cucina/{id}```
Restituisce il dettaglio di una stazione di lavoro, identificata dal suo id (numerico)
### ```GET /cucina/{id}/ordine (elenco piatti da preparare)```
Restituisce tutti i prodotti da preparare presso una stazione di lavoro, identificata dal suo id (numerico)
### ```POST /cucina/{id}/ordine (dichiara ordine preparato)```
Imposta alcuni prodotti come preparati (presso una stazione di lavoro)
```json
[
    {
        "id": 4
    }
]
```

## API per applicazione Statistiche
**NB** Tutte le API richiedono l'invio del token JWT

### ```GET /statistiche/tavoli```
Restituisce elenco dei tavoli
### ```GET /statistiche/ordini```
Restituisce l'elenco dei prodotti ordinati nella giornata odierna