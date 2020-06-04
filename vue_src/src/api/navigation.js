import request from '@/utils/request'

export function fetchList(query) {
  return request({
    url: '/api/Article/GetList',
    method: 'get',
    params: query
  })
}
