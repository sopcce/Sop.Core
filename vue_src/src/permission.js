import router from './router'
import store from './store'
import { Message } from 'element-ui'
import NProgress from 'nprogress' // progress bar
import 'nprogress/nprogress.css' // progress bar style
import { getToken } from '@/utils/auth' // get token from cookie
import getPageTitle from '@/utils/get-page-title'

NProgress.configure({ showSpinner: false }) // NProgress Configuration

const whiteList = ['/login', '/auth-redirect'] // no redirect whitelist

/**
 * 合并路由
 * @param {arr} clientAsyncRoutes 前端保存动态路由
 * @param {arr} serverRouter 后端保存动态路由
 */
function makePermissionRouters(serverRouter, clientAsyncRoutes) {
  clientAsyncRoutes.map(ele => {
    if (!ele.name || (!ele.meta && !ele.meta.roles)) return
    // let roles_obj
    // for (let i = 0; i < serverRouter.length; i++) {
    //   const element = serverRouter[i]
    //   if (ele.name === element.name) {
    //     roles_obj = element
    //   }
    // }
    // ele.meta.roles = roles_obj.meta.roles

    if (ele.children) {
      makePermissionRouters(serverRouter, ele.children)
    }
  })
  return clientAsyncRoutes
}

router.beforeEach(async(to, from, next) => {
  // start progress bar
  NProgress.start()

  // set page title
  document.title = getPageTitle(to.meta.title)

  // determine whether the user has logged in
  const hasToken = getToken()

  if (hasToken) {
    if (to.path === '/login') {
      // if is logged in, redirect to the home page
      next({ path: '/' })
      NProgress.done() // hack: https://github.com/PanJiaChen/vue-element-admin/pull/2939
    } else {
      // determine whether the user has obtained his permission roles through getInfo
      const hasRoles = store.getters.roles && store.getters.roles.length > 0
      if (hasRoles) {
        next()
      } else {
        try {
          // 获取用户信息
          // 注意：角色必须是对象数组！例如：['admin']或，['developer'，'editor']
          const { roles } = await store.dispatch('user/getInfo')

          // generate accessible routes map based on roles
          const accessRoutes = await store.dispatch(
            'permission/generateRoutes',
            roles
          )

          // ### todo add
          debugger
          console.log(accessRoutes)
          const serverRouter = [
            {
              path: '/managener',
              name: 'Managener',
              meta: {
                title: '各平台管理',
                icon: 'example',
                roles: ['admin', 'editor']
              }
            }
          ]
          makePermissionRouters(serverRouter, accessRoutes)
          // accessRoutes = makePermissionRouters(serverRouter, accessRoutes)
          console.log(accessRoutes)
          // ### todo add end

          // 动态添加可访问路由
          router.addRoutes(accessRoutes)

          // 确保addRoutes完整的hack方法
          // 设置replace:true，这样导航就不会留下历史记录
          next({ ...to, replace: true })
        } catch (error) {
          // remove token and go to login page to re-login
          await store.dispatch('user/resetToken')
          Message.error(error || 'Has Error')
          next(`/login?redirect=${to.path}`)
          NProgress.done()
        }
      }
    }
  } else {
    /* has no token*/

    if (whiteList.indexOf(to.path) !== -1) {
      // in the free login whitelist, go directly
      next()
    } else {
      // other pages that do not have permission to access are redirected to the login page.
      next(`/login?redirect=${to.path}`)
      NProgress.done()
    }
  }
})

router.afterEach(() => {
  // finish progress bar
  NProgress.done()
})
