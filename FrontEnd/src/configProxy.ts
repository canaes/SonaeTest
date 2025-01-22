const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function(app: any) {
  app.use(
    '/api',
    createProxyMiddleware({
      target: 'https://localhost:7043',
      changeOrigin: true,
      secure: false
    })
  );
};
