mergeInto(LibraryManager.library, {
  SaveToLocalStorage: function (key, value) {
    key = Pointer_stringify(key);
    value = Pointer_stringify(value);
    window.localStorage.setItem(key, value);
  },
  LoadFromLocalStorage: function (key) {
    key = Pointer_stringify(key);
    var value = window.localStorage.getItem(key);
    var bufferSize = lengthBytesUTF8(value) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(value, buffer, bufferSize);
    return buffer;
  },
  SyncFiles: function () {},
  WindowAlert: function (message) {
    message = Pointer_stringify(message);
    window.alert(message);
  },
});
