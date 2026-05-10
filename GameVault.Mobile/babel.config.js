module.exports = function (api) {
  api.cache(true);

  return {
    presets: [[
      'babel-preset-expo', {jsximportSource: 'nativewind'},
    ], "nativewind/babel"],
  };
};