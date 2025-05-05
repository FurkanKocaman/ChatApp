export interface PaginatedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCOunt: number;
}
