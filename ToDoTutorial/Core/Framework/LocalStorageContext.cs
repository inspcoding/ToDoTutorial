using Blazored.LocalStorage;

namespace ToDoTutorial.Core.Framework;

public class LocalStorageContext(ILocalStorageService localStorage)
{
    public async Task SetItemAsync<T>(string storageName, T item) => 
        await localStorage.SetItemAsync<T>(storageName, item);

    public async Task SetItemsAsync<T>(string storageName, List<T> items) => 
        await localStorage.SetItemAsync<List<T>>(storageName, items);

    public async Task<T> GetItemAsync<T>(string storageName) => 
        await localStorage.GetItemAsync<T>(storageName);

    public async Task<List<T>> GetItemsAsync<T>(string storageName) => 
        await localStorage.GetItemAsync<List<T>>(storageName);
}
