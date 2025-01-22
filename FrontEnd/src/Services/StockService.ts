
export const fetchAvStock = async (): Promise<number> => {
  const response = await fetch('https://localhost:7043/api/v1/stock');
  const data = await response.json();
  console.log(data);  // Adicione um log para garantir que os dados estão corretos

  return data.data as number;  
}