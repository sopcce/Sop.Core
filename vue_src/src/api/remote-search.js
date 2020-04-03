import request from '@/utils/request'

export function searchUser(name) {
  return request({
    url: '/api/user/SearchUser',
    method: 'get',
    params: { name }
  })
}

export function transactionList(query) {
  return request({
    url: '/api/Home/GetList',
    method: 'get',
    params: query
  })
}
