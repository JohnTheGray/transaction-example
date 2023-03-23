# transaction-example

## Oveview

A simple row and column data store that supports transactions (non-nested). The transaction cache has two implementations (`TransactionDictionaryCache` and `TransactionOperationCache`) in order explore
the differences between storing transaction changes are a list of operations vs. a simplified dictionary of dictionaries.

### TransactionDictionaryCache

Stores the transaction changes in a dictionary of dictionaries along with a separate set that tracks what rows where deleted.

### TransactionOperationCache

Stores the transaction changes as a list of operation for each row. These operations are replayed on the backing row to produce the final row to commit or print.
