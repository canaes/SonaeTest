
export const fetchAvStock = async (): Promise<number> => {
  const response = await fetch('https://localhost:7043/api/v1/stock');
  const data = await response.json();

  return data.data as number;  
}