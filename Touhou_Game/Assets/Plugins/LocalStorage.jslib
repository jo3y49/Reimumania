mergeInto(LibraryManager.library, {
  SaveToLocalStorage: function (key, value) {
    key = UTF8ToString(key);
    value = UTF8ToString(value);
    window.localStorage.setItem(key, value);
  },
  LoadFromLocalStorage: function (key) {
    key = UTF8ToString(key);
    var value = window.localStorage.getItem(key);
    if (value === null) {
      return 0;
    }
    var bufferSize = lengthBytesUTF8(value) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(value, buffer, bufferSize);
    return buffer;
  },
  SyncFiles: function () {},
  WindowAlert: function (message) {
    message = UTF8ToString(message);
    window.alert(message);
  },
  RemoveFromLocalStorage: function (key) {
    key = UTF8ToString(key);
    window.localStorage.removeItem(key);
  },
});
