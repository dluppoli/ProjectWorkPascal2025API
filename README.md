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

### ```GET /waiter/tables```
Restituisce elenco dei tavoli
### ```GET /waiter/tables/{id}```
Restituisce le informazioni di un tavolo dato il suo id (numerico)
### ```PUT /waiter/tables/{id}```
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
### ```GET /waiter/tables/{id}/bill```
Restituisce il conto del tavolo (identificato con il suo id numerico), composto dal totale e dal dettaglio di tutti gli ordini "a pagamento"
### ```GET /waiter/tables/{id}/order```
Restituisce tutti i prodotti ordinati dal tavolo (identificato con il suo id numerico), inserendo anche quelli compresi nel menù all you can eat
### ```POST /waiter/tables/{id}/order```
Aggiunge uno o più prodotti all'ordine del tavolo  (identificato con il suo id numerico)
```json
[
    {
        "productId": 76,
        "qty": 2
    }
]
```
### ```DELETE /waiter/tables/{id}/order```
Cancella tutti i piatti ordinati da un tavolo (identificato con il suo id numerico)
### ```DELETE /waiter/tables/{id}/order/{ido} (per cancellare l'ordine di un piatto)```
Cancella tutti un piatto (identificato con il suo id numerico) dall'ordine di un tavolo (identificato con il suo id numerico)
### ```GET /waiter/products```
Restituisce l'elenco di tutti i prodotti
### ```GET /waiter/products/{id}```
Restituisce il dettaglio di un prodotto, dato il suo id (numerico)
### ```GET /waiter/categories```
Restituisce l'elenco di tutte le categorie di prodotto
### ```GET /waiter/categories/{id}```
Restituisce il dettaglio di una categoria, dato il suo id (numerico)
### ```GET /waiter/categories/{id}/products```
Restituisce tutti i prodotti presenti in una categoria, dato il suo id (numerico)


## API per applicazione Cliente
**NB** Tutte le API richiedono l'invio dell'Api Key (Header APIKey)

### ```GET /customer/products```
Restituisce l'elenco di tutti i prodotti
### ```GET /customer/products/{id}```
Restituisce il dettaglio di un prodotto, dato il suo id (numerico)
### ```GET /customer/categories```
Restituisce l'elenco di tutte le categorie di prodotto
### ```GET /customer/categories/{id}```
Restituisce il dettaglio di una categoria, dato il suo id (numerico)
### ```GET /customer/categories/{id}/products```
Restituisce tutti i prodotti presenti in una categoria, dato il suo id (numerico)
### ```GET /customer/bill```
Restituisce il conto del tavolo, composto dal totale e dal dettaglio di tutti gli ordini "a pagamento"
### ```GET /customer/order```
Restituisce tutti i prodotti ordinati dal tavolo, inserendo anche quelli compresi nel menù all you can ea
### ```POST /customer/order```
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

### ```GET /kitchen/```
Restituisce tutte le stazioni di lavoro
### ```GET /kitchen/{id}```
Restituisce il dettaglio di una stazione di lavoro, identificata dal suo id (numerico)
### ```GET /kitchen/{id}/order (elenco piatti da preparare)```
Restituisce tutti i prodotti da preparare presso una stazione di lavoro, identificata dal suo id (numerico)
### ```POST /kitchen/{id}/order (dichiara ordine preparato)```
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

### ```GET /stats/tables```
Restituisce elenco dei tavoli
### ```GET /stats/order```
Restituisce l'elenco dei prodotti ordinati nella giornata odierna