# Chaikin Money Flow (CMF)

Created by Marc Chaikin, [Chaikin Money Flow](https://en.wikipedia.org/wiki/Chaikin_Analytics#Chaikin_Money_Flow) is the simple moving average of the Money Flow Volume.
[[Discuss] :speech_balloon:](https://github.com/DaveSkender/Stock.Indicators/discussions/261 "Community discussion about this indicator")

![image](chart.png)

```csharp
// usage
IEnumerable<CmfResult> results =
  quotes.GetCmf(lookbackPeriods);  
```

## Parameters

| name | type | notes
| -- |-- |--
| `lookbackPeriods` | int | Number of periods (`N`) in the moving average.  Must be greater than 0.  Default is 20.

### Historical quotes requirements

You must have at least `N+1` periods of `quotes`.

`quotes` is an `IEnumerable<TQuote>` collection of historical price quotes.  It should have a consistent frequency (day, hour, minute, etc).  See [the Guide](../../docs/GUIDE.md#historical-quotes) for more information.

## Response

```csharp
IEnumerable<CmfResult>
```

The first `N-1` periods will have `null` values since there's not enough data to calculate.  We always return the same number of elements as there are in the historical quotes.

### CmfResult

| name | type | notes
| -- |-- |--
| `Date` | DateTime | Date
| `MoneyFlowMultiplier` | decimal | Money Flow Multiplier
| `MoneyFlowVolume` | decimal | Money Flow Volume
| `Cmf` | decimal | Chaikin Money Flow = SMA of MFV for `N` lookback periods

:warning: **Warning**: absolute values in MFV and CMF are somewhat meaningless, so use with caution.

### Utilities

- [.Find(lookupDate)](../../docs/UTILITIES.md#find-indicator-result-by-date)
- [.RemoveWarmupPeriods()](../../docs/UTILITIES.md#remove-warmup-periods)
- [.RemoveWarmupPeriods(qty)](../../docs/UTILITIES.md#remove-warmup-periods)

See [Utilities and Helpers](../../docs/UTILITIES.md#content) for more information.

## Example

```csharp
// fetch historical quotes from your feed (your method)
IEnumerable<Quote> quotes = GetHistoryFromFeed("SPY");

// calculate 20-period CMF
IEnumerable<CmfResult> results = quotes.GetCmf(20);

// use results as needed
CmfResult result = results.LastOrDefault();
Console.WriteLine("CMF on {0} was {1}", result.Date, result.Cmf);
```

```bash
CMF on 12/31/2018 was -20257894
```
