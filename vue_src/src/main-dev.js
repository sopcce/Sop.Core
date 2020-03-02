import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import './plugins/element.js'
import moment from 'moment'
// 引入ZkTable
import './assets/css/global.css'
import './assets/font/iconfont.css'
import ZkTable from 'vue-table-with-tree-grid'

// 引入v-viewer
import 'viewerjs/dist/viewer.css'
import Viewer from 'v-viewer'
// 引入axios
import axios from 'axios'

// 引入富文本编辑器
import VueQuillEditor from 'vue-quill-editor'
// require styles
import 'quill/dist/quill.core.css'
import 'quill/dist/quill.snow.css'
import 'quill/dist/quill.bubble.css'
// 导入nprogress
import NProgress from 'nprogress'
import 'nprogress/nprogress.css'

// http://119.23.53.78:8888/api/private/v1
// http://119.3.183.37:8888/api/private/v1/
axios.defaults.baseURL = 'https://www.liulongbin.top:8888/api/private/v1/'
// 在request拦截器中显示进度条 NProgress.start()
axios.interceptors.request.use(config => {
  NProgress.start()
  config.headers.Authorization = window.sessionStorage.getItem('token')
  return config
})
// 在response拦截器中隐藏进度条 NProgress.done()
axios.interceptors.response.use(config => {
  NProgress.done()
  return config
})
Vue.prototype.$http = axios

Vue.filter('dateFormat', (dateStr, pattern = 'YYYY-MM-DD HH:mm:ss') => {
  return moment(dateStr).format(pattern)
})

Vue.config.productionTip = false
Vue.component('tree-table', ZkTable)
Vue.use(Viewer)
Vue.use(VueQuillEditor)

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
